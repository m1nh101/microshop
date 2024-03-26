import { Component } from '@angular/core';
import { FormControl, FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms';
import { UserCredential, UserService } from '../../services/userService';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  username: string = "";
  password: string = "";

  formValidation: FormGroup<{
    username: FormControl<string>;
    password: FormControl<string>;
  }>;

  constructor(
    private formBuilder: NonNullableFormBuilder,
    private userService: UserService) {
    this.formValidation = this.formBuilder.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }


  onLoginClick(): void {
    if(this.formValidation.valid) {
      console.log(this.formValidation.value)

      this.userService.Authenticate(this.formValidation.value as UserCredential)
        .subscribe(res => {
          if(res.isSuccess) {
            localStorage.setItem('access_token', res.data.accessToken); // will be store by http cookie
            localStorage.setItem('refresh_token', res.data.refreshToken);
          }
        })

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
