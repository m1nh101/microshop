import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Result } from "./common";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";

interface UserCredential {
  username: string;
  password: string;
}

interface UserRegister extends UserCredential {
  email: string;
  confirmPassword: string;
  name: string;
}

interface AuthenticationResponse {
  accessToken: string;
  refreshToken: string;
}

@Injectable({
  providedIn: 'root'
})
class UserService {
  constructor(private httpClient: HttpClient) {
  }

  Authenticate(payload: UserCredential): Observable<Result<AuthenticationResponse>> {
    const header: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.httpClient.post<Result<AuthenticationResponse>>('https://localhost:7168/user-api/users/auth',
      JSON.stringify(payload),
      {
        headers: header
      });
  }

  Register(payload: UserRegister): Observable<Result<boolean>> {
    const header: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.httpClient.post<Result<boolean>>('',
      JSON.stringify(payload),
      {
        headers: header
      });
  }
}

export { UserService, UserRegister, UserCredential }