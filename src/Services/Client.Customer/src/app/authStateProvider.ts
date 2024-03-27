import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
class AuthStateProvider {
  private accessToken: string = "";
  private userName: string = "";
  private role: string = "";

  constructor(){}

  setAccessToken(token: string): void {
    this.accessToken = token;
  }

  setUserName(userName: string): void {
    this.userName = userName;
  }

  setRole(role: string): void {
    this.role = role;
  }

  getAccessToken(): string {
    return this.accessToken;
  }

  getUserName(): string {
    return this.userName;
  }

  getRole(): string {
    return this.role;
  }

  isAuthenticated(): boolean {
    return this.accessToken !== "";
  }
}

export { AuthStateProvider }