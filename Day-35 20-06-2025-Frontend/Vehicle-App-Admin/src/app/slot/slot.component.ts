import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, FormsModule, Validators } from '@angular/forms';
import { SlotService } from '../services/slot.service';
import { UserService } from '../services/user.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-slot',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './slot.component.html',
  styleUrl: './slot.component.css'
})
export class SlotComponent implements OnInit {
  errorMessage: string = '';
  slots: any[] = [];
  mechanics: any[] = [];

  isModalOpen: boolean = false;
  modalMode: 'edit' | 'add' = 'add';
  slotForm!: FormGroup;
  selectedSlotId: number | null = null;
  currentDateTime: string = '';

  constructor(
    private slotService: SlotService,
    private userService: UserService,
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.setMinDateTime();
    this.loadSlots();
    this.loadMechanics();

    this.slotForm = this.fb.group({
      slotDateTime: [null, Validators.required],
      mechanicID: [null, Validators.required],
      status: ['available', Validators.required]
    });
  }

  setMinDateTime(): void {
    const now = new Date();
    now.setMinutes(now.getMinutes() - now.getTimezoneOffset()); // Ensure local time
    this.currentDateTime = now.toISOString().slice(0, 16);
  }

  loadMechanics(): void {
    this.userService.getAllMechanics().subscribe({
      next: (response) => {
        this.mechanics = response;
      },
      error: (error) => {
        this.errorMessage = error?.error?.error?.error ?? 'Error in Getting Mechanics';
      }
    });
  }

  loadSlots(): void {
    this.slotService.getAll().subscribe({
      next: (response) => {
        this.slots = response.sort((a: any, b: any) =>
          new Date(a.slotDateTime).getTime() - new Date(b.slotDateTime).getTime()
        );
      },
      error: (error) => {
        this.errorMessage = error?.error?.error?.error ?? 'Error in Getting Service Slots';
      }
    });
  }

  openAddModal(): void {
    this.modalMode = 'add';
    this.selectedSlotId = null;
    this.slotForm.reset({ status: 'available' });
    this.isModalOpen = true;
  }

  openEditModal(slot: any): void {
    this.modalMode = 'edit';
    this.selectedSlotId = slot.id;
    this.slotForm.patchValue({
      slotDateTime: this.formatForInput(slot.slotDateTime),
      mechanicID: slot.mechanicID,
      status: slot.status
    });
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
  }

  saveSlot(): void {
    if (this.slotForm.invalid) return;

    const payload = {
      ...this.slotForm.value,
      slotDateTime: new Date(this.slotForm.value.slotDateTime).toISOString()
    };

    const obs = this.modalMode === 'edit' && this.selectedSlotId
      ? this.slotService.update(this.selectedSlotId, payload)
      : this.slotService.create(payload);

    obs.subscribe({
      next: () => {
        this.loadSlots();
        this.closeModal();
      },
      error: () => {
        this.errorMessage = this.modalMode === 'edit'
          ? 'Failed to update slot.'
          : 'Failed to add slot.';
      }
    });
  }

  deleteSlot(): void {
    if (this.selectedSlotId !== null) {
      this.slotService.delete(this.selectedSlotId).subscribe({
        next: () => {
          this.closeModal();
          this.loadSlots();
        },
        error: (error) => {
          this.errorMessage = error?.error?.error?.error ?? 'Error in Deleting Slot';
        }
      });
    }
  }

  private formatForInput(dateStr: string): string {
    const dt = new Date(dateStr);
    return dt.toISOString().slice(0, 16); // For datetime-local
  }
}
