import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { JwtModule } from '@auth0/angular-jwt';
import { environment } from '../environments/environment';
import { NavModule } from './nav/nav.module';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AjoutComponent } from './admin/ajout/ajout.component';
import { NgxMaskDirective, provideEnvironmentNgxMask } from 'ngx-mask';
import { ResponseErrorHandlerInterceptor } from './interceptors/response-error-handler.interceptor';

export function tokenGetter() {
    const token = localStorage.getItem('token');
    return `Bearer ${token}`; // Add the "Bearer" prefix
}

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        HomeComponent,
        NotFoundComponent,
        AjoutComponent,
    ],
    imports: [
        BrowserModule,
        FormsModule,
        ReactiveFormsModule,
        AppRoutingModule,
        NgxMaskDirective,
        JwtModule.forRoot({
            config: {
                tokenGetter,
                allowedDomains: [environment.apiUrl],
                headerName: 'Authorization', // Set the header name
            },
        }),
        HttpClientModule,
        NavModule,
    ],
    providers: [
        provideEnvironmentNgxMask(),
        {
            provide: HTTP_INTERCEPTORS,
            useClass: ResponseErrorHandlerInterceptor,
            multi: true,
        },
    ],
    bootstrap: [AppComponent],
})
export class AppModule {}
