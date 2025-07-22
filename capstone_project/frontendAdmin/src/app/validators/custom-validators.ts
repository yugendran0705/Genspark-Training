import { AbstractControl, ValidationErrors, ValidatorFn, FormGroup } from '@angular/forms';


export class CustomValidators {

  static passwordStrength(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value || '';
      const isStrong = value.length >= 6 && /\d/.test(value) && /[!@#$%^&*]/.test(value);
      return isStrong ? null : { weakPassword: true };
    };
  }

  static matchPassword(passwordControl: string, confirmControl: string): ValidatorFn {
    return (group: AbstractControl): ValidationErrors | null => {
      const password = group.get(passwordControl)?.value;
      const confirm = group.get(confirmControl)?.value;
      return password === confirm ? null : { passwordMismatch: true };
    };
  }
}