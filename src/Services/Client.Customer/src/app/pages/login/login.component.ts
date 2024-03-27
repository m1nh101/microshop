import { Component } from '@angular/core';
import { FormControl, FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms';
import { UserCredential, UserService } from '../../services/userService';
import { AuthStateProvider } from '../../authStateProvider';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  username: string = "";
  password: string = "";
  loginFailed: boolean = false;

  formValidation: FormGroup<{
    username: FormControl<string>;
    password: FormControl<string>;
  }>;

  constructor(
    private formBuilder: NonNullableFormBuilder,
    private userService: UserService,
    private authProvider: AuthStateProvider,
    private router: Router) {
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
            this.authProvider.setAccessToken(res.data.accessToken); 
            localStorage.setItem('refresh_token', res.data.refreshToken); // will be store by http cookie
            this.router.navigate(['/'])
            return;
          }

          this.loginFailed = true;
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
