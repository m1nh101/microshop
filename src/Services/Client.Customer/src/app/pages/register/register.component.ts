import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, NonNullableFormBuilder, ValidatorFn, Validators } from '@angular/forms';
import { UserRegister, UserService } from '../../services/userService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  name: string = "";
  username: string = "";
  password: string = "";
  confirmPassword: string = "";
  email: string = "";

  formValidation: FormGroup<{
    username: FormControl<string>;
    password: FormControl<string>;
    confirmPassword: FormControl<string>;
    email: FormControl<string>;
    name: FormControl<string>;
  }>;

  constructor(
    private formBuilder: NonNullableFormBuilder,
    private userService: UserService,
    private router: Router) {
    this.formValidation = this.formBuilder.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
      confirmPassword: ['', [Validators.required, this.confirmationValidator]],
      email: ['', [Validators.required]],
      name: ['', [Validators.required]]
    });
  }

  updateConfirmValidator(): void {
    /** wait for refresh value */
    Promise.resolve().then(() => this.formValidation.controls.confirmPassword.updateValueAndValidity());
  }

  confirmationValidator: ValidatorFn = (control: AbstractControl): { [s: string]: boolean } => {
    if (!control.value) {
      return { required: true };
    } else if (control.value !== this.formValidation.controls.password.value) {
      return { confirm: true, error: true };
    }
    return {};
  };

  onRegisterClick(): void {
    if(this.formValidation.valid) {
      this.userService.Register(this.formValidation.value as UserRegister)
        .subscribe(res => {
          if(res.isSuccess) {
            this.router.navigate(['/login']);
            return;
          }
        });

      return;
    }

    Object.values(this.formValidation.controls).forEach(control => {
      if (control.invalid) {
        control.markAsDirty();
        control.updateValueAndValidity({ onlySelf: true });
      }
    });
  }
}
