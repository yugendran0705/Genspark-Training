import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { UserService } from '../services/user.service';
import { User } from '../models/user.model';
import { CommonModule } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ReactiveFormsModule } from '@angular/forms';


@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.css'],
  imports: [CommonModule, ReactiveFormsModule]
})
export class UserFormComponent implements OnInit {
  userForm!: FormGroup;
  bannedWords = ['admin', 'root'];

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.userForm = this.fb.group({
      username: ['', [Validators.required, this.bannedWordsValidator(this.bannedWords)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8), this.passwordStrengthValidator()]],
      confirmPassword: ['', Validators.required],
      role: ['User', Validators.required]
    }, { validators: this.matchPasswords });
  }

  bannedWordsValidator(banned: string[]): ValidatorFn {
    return (control: AbstractControl) => {
      if (!control.value) return null;
      const hasBanned = banned.some(word => control.value.toLowerCase().includes(word));
      return hasBanned ? { bannedName: true } : null;
    };
  }

  matchPasswords(group: AbstractControl) {
    const password = group.get('password')?.value;
    const confirm = group.get('confirmPassword')?.value;
    if (password !== confirm) {
      group.get('confirmPassword')?.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    }
    return null;
  }

  passwordStrengthValidator(): ValidatorFn {
    return (control: AbstractControl) => {
      const value = control.value;
      if (!value) return null;
      const hasNumber = /[0-9]+/.test(value);
      const hasSymbol = /[!@#$%^&*(),.?":{}|<>]+/.test(value);
      const valid = hasNumber && hasSymbol && value.length >= 8;
      return !valid ? { weakPassword: true } : null;
    };
  }

  onSubmit(): void {
    if (this.userForm.valid) {
      const { username, email, password, role } = this.userForm.value;
      const newUser: User = { username, email, password, role };
      this.userService.addUser(newUser);
      this.snackBar.open('User Added Successfully!', 'Close', { duration: 3000 });
      this.userForm.reset();
    }
  }
}
