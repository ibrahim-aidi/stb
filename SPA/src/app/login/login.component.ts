import { Component } from '@angular/core';
import { ServiceResponse } from '../DTOS/ServiceResponse';
import { AuthService } from '../Services/auth.service';
import { UserLogin } from '../DTOS/UserLogin';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  user: UserLogin  = {
    login : '',
    password: ''
  };

  showAlert = false
  alertMsg = 'Veuillez patienter pendant que nous vous connectons.'
  alertColor = 'blue'
  inSubmission = false

  constructor(private auth: AuthService, private router:Router) { }

  login() {
    this.showAlert = true;
    this.alertMsg = 'Veuillez patienter pendant que nous vous connectons.';
    this.alertColor = 'blue';
    this.inSubmission = true;

    this.auth.login(this.user).subscribe({
      next: (response: ServiceResponse<string>) => {
        this.alertColor = 'green'
        this.handleLoginResponse(response);
        setTimeout(() => {
          this.router.navigate([''])
        }, 1);
      },
      error: (error) => {
        this.alertColor = 'red'
        this.handleLoginResponse(error);

      },
      complete: () => {
        // Facultatif : Gérez la logique de complétion si nécessaire
      }
    });
  }

  private handleLoginResponse(response: ServiceResponse<string>): void {
    this.alertMsg = response.message;
    this.inSubmission = false;
  }
}
