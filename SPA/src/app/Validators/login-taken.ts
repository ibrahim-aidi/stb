import { Injectable } from '@angular/core';
import { AbstractControl, AsyncValidator, ValidationErrors } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from '../Services/auth.service';


@Injectable({
  providedIn: 'root'
})
export class LoginTaken implements AsyncValidator {
  constructor(private auth: AuthService) {}

  validate = (control: AbstractControl): Observable<ValidationErrors | null> => {
    const login = control.value;

    return this.auth.checkLoginExists(login).pipe(
      map(exists => (exists ? { LoginTaken: true } : null)),
    );
  }
}
