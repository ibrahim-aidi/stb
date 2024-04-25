import { Component } from '@angular/core';
import { UserRegister } from '../../DTOS/UserRegister';
import { ServiceResponse } from '../../DTOS/ServiceResponse';
import { AuthService } from '../../Services/auth.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { EmailTaken } from '../../Validators/email-taken';
import { LoginTaken } from '../../Validators/login-taken';

@Component({
  selector: 'app-ajout',
  templateUrl: './ajout.component.html',
  styleUrl: './ajout.component.css'
})
export class AjoutComponent {
  constructor(private auth: AuthService,
    private emailTaken: EmailTaken,
    private loginTaken: LoginTaken) {}

  firstName = new FormControl('', [Validators.required, Validators.minLength(4)]);
  lastName = new FormControl('', [Validators.required, Validators.minLength(4)]);
  email = new FormControl('', [ Validators.required,Validators.email], [this.emailTaken.validate]);
  login = new FormControl('', [ Validators.required,Validators.minLength(4)], [this.loginTaken.validate]);
  telephone = new FormControl('', [Validators.required, Validators.pattern(/^\d{8}$/)]);
  role = new FormControl('User', [Validators.required, Validators.pattern(/^(Admin|User)$/)]);
  etat = new FormControl(true, [Validators.required, Validators.pattern(/^(Actif|Non Actif)$/)]);

  registerForm = new FormGroup({
    firstName: this.firstName,
    lastName: this.lastName,
    login: this.login,
    email: this.email,
    telephone: this.telephone,
    role: this.role,
    etat: this.etat,
  });

  showAlert = false;
  alertMsg = 'Please wait! Your account is being created.';
  alertColor = 'blue';
  submissionInProgress = false;

  register() {
    this.showAlert = true;
    this.alertMsg = 'Please wait while we create your account.';
    this.alertColor = 'blue';
    this.submissionInProgress = true;

    this.auth.register(this.registerForm.value as UserRegister).subscribe({
      next: (response: ServiceResponse<number>) => {
        this.alertColor = 'green';
        this.handleRegisterResponse(response);
      },
      error: (error: ServiceResponse<number>) => {
        this.alertColor = 'red';
        this.handleRegisterResponse(error);
      },
      complete: () => {
        this.submissionInProgress = false;
      }
    });
  }

  private handleRegisterResponse(response: ServiceResponse<number>): void {
    this.alertMsg = response.message;
  }
}
