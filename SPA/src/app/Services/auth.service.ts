import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Observable, map, of } from "rxjs";
import { UserLogin } from "../DTOS/UserLogin";
import { ServiceResponse } from "../DTOS/ServiceResponse";
import { UserRegister } from "../DTOS/UserRegister";
import { Router } from "@angular/router";





@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly BaseUrl = environment.apiUrl + '/auth';

  constructor(private http: HttpClient,
              private jwtHelper: JwtHelperService,
              private router:Router) {}

  checkEmailExists(email: string): Observable<boolean> {
    const url = `${this.BaseUrl}/check-email-exists/${email}`
    return this.http.get<boolean>(url);
  }

  checkLoginExists(Login: string): Observable<boolean> {
    const url = `${this.BaseUrl}/check-login-exists/${Login}`
    return this.http.get<boolean>(url);
  }

  isUserAuthenticated(): Observable<boolean>
  {
    const token = localStorage.getItem('token')
    if (token && !this.jwtHelper.isTokenExpired(token)){
      return of(true);
    }
    else {
      return of(false)
    }
  }

  isAdmin(): Observable<boolean> {
    const token = localStorage.getItem('token');
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      // Assuming the role is stored in the token under the key 'role'
      const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      // Check if the role is 'admin'
      return of(role === 'Admin');
    } else {
      return of(false);
    }
  }

  login(request: UserLogin): Observable<ServiceResponse<string>> {
    const url = `${this.BaseUrl}/login`
    return this.http.post<ServiceResponse<string>>
      (url, request).pipe(
      map((response: ServiceResponse<string>) => {
        if (!response.data) {
          throw new Error('Wrong credentials');
        }
        localStorage.setItem('token', response.data);
        return response;
      })
    );
  }

  register(request: UserRegister): Observable<ServiceResponse<number>> {
    const url = `${this.BaseUrl}/register`
    return this.http.post<ServiceResponse<number>>(url, request);
  }

  logout($event?: Event)
  {
    if ($event) {
      $event.preventDefault();
    }
    localStorage.removeItem("token");
    this.router.navigateByUrl('login');
  }


}
