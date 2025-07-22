import { AbstractControl, ValidationErrors, ValidatorFn, FormGroup, AsyncValidatorFn } from '@angular/forms';
import { Loginservice } from '../services/loginservice';
import { map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';


export class CustomValidators {

    constructor() {

    }

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

    static userExists(loginservice: Loginservice): AsyncValidatorFn {
        return (control: AbstractControl) => {
            if (!control.value) {
                return of(null);
            }
            return loginservice.checkUserExists(control.value).pipe(
                map((data: any) => (data && data.email === control.value ? { userExists: true } : null)),
                catchError(() => of(null))
            );
        };
    }
}