import { Component, OnInit } from '@angular/core';
import { ProfileService } from '../services/profile-service';
import { WalletService } from '../services/wallet-service';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule,ReactiveFormsModule,ValidationErrors,Validators } from '@angular/forms';
//@ts-ignore
import * as bootstrap from 'bootstrap';



@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule,ReactiveFormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class Profile implements OnInit {
  userdata: any = {};
  walletBalance: number = 0;
  ticketCount: number = 0;
  editData: any = {};
  editFormGroup!:FormGroup;
  addAmount: number = 0;
  deductAmount: number = 0;

  submitAddFunds() {
    if (!this.addAmount || this.addAmount <= 0) return;

    const dto = {
      email: this.userdata.email,
      amount: this.addAmount
    };

    this.walletService.addFunds(dto).subscribe({
      next: (wallet) => {
        this.walletBalance = wallet.balance;
        this.addAmount = 0;

        const modalEl = document.getElementById('addFundsModal');
        if (modalEl) {
          const modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
          modal.hide();
        }
      },
      error: (err) => {
        console.error('Add funds failed:', err);
        alert('Failed to add funds.');
      }
    });
  }

  submitDeductFunds() {
    if (!this.deductAmount || this.deductAmount <= 0) return;
    if (this.deductAmount > this.walletBalance) {
      alert('Insufficient funds to deduct this amount.');
      return;
    }

    const dto = {
      email: this.userdata.email,
      amount: this.deductAmount
    };

    this.walletService.deductFunds(dto).subscribe({
      next: (wallet) => {
        this.walletBalance = wallet.balance;
        this.deductAmount = 0;

        const modalEl = document.getElementById('deductFundsModal');
        if (modalEl) {
          const modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
          modal.hide();
        }
      },
      error: (err) => {
        console.error('Deduct funds failed:', err);
        alert('Failed to deduct funds.');
      }
    });
  }


  constructor(
    private profileservice: ProfileService,
    private fb:FormBuilder,
    private walletService: WalletService
  ) { }

  ngOnInit(): void {
    this.editFormGroup = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: [{ value: '', disabled: true }, [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^[0-9]{10}$/)]]
    });
    const email = localStorage.getItem('username');
    if (email) {
      this.profileservice.getUserProfile().subscribe({
        next: (data: any) => {
          this.userdata = data;
          this.ticketCount = data.tickets?.length || 0;
          this.profileservice.setcustomername(data.name);
        },
        error: (err: any) => {
          console.error('Failed to load profile:', err);
        }
      });
      this.walletService.getWalletByEmail(email).subscribe({
        next: (wallet: any) => {
          this.walletBalance = wallet.balance;
        },
        error: (err) => {
          console.error('Failed to load wallet:', err);
        }
      });
    }
  }

  openEditModal() {
    this.editFormGroup = this.fb.group({
      name: [this.userdata.name, [Validators.required, Validators.minLength(3)]],
      email: [{ value: this.userdata.email, disabled: true }, [Validators.required, Validators.email]],
      phoneNumber: [this.userdata.phoneNumber, [Validators.required, Validators.pattern(/^[0-9]{10}$/)]]
    });
  }

  submitEdit() {
    if (this.editFormGroup.valid) {
      const updatedData = {
        ...this.userdata,
        name: this.editFormGroup.value.name,
        phoneNumber: this.editFormGroup.value.phoneNumber
      };

      this.profileservice.updateUserProfile(updatedData).subscribe({
        next: (data: any) => {
          this.userdata = data;
          this.profileservice.setcustomername(data.name);
          const modalEl = document.getElementById('editProfileModal');
          if (modalEl) {
            const modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
            modal.hide();
          }
        },
        error: (err: any) => {
          alert(err.error);
          console.log(err);
        }
      });
    }
  }
}
