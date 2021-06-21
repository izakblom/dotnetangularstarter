import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { AngularFireModule } from '@angular/fire';
import { AngularFireAuthModule } from '@angular/fire/auth';
import { AngularFirestoreModule } from '@angular/fire/firestore';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ClarityModule } from '@clr/angular';
import { environment } from 'src/environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthenticationModule } from './authentication/authentication.module';
import { ClientPagesModule } from './client-pages/client-pages.module';
import { DataTableModule } from './data-table/data-table.module';
import { DynamicMenuModule } from './dynamic-menu/dynamic-menu.module';
import { HomeModule } from './home/home.module';
import { MaterialModule } from './material/material.module';
import { ProfileModule } from './profile/profile.module';
import { AdminService } from './services/admin.service';
import { ApiService } from './services/api.service';
import { AuthService } from './services/auth.service';
import { LocalStorageService } from './services/localStorage.service';
import { NotifyService } from './services/notifyService';
import { PermissionsService } from './services/permissions.service';
import { UserService } from './services/user.service';
import { TickersModule } from './shared/tickers/tickers.module';



@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    AngularFireModule.initializeApp(environment.firebase),
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    AngularFireAuthModule,
    AngularFirestoreModule,
    AuthenticationModule,
    MaterialModule,
    HomeModule,
    ProfileModule,
    DynamicMenuModule,
    DataTableModule,
    HttpClientModule,
    ClarityModule,
    TickersModule,
    ClientPagesModule,
  ],
  providers: [
    AuthService,
    ApiService,
    UserService,
    LocalStorageService,
    NotifyService,
    AdminService,
    PermissionsService,
  ],
  bootstrap: [AppComponent],
  exports: []
})
export class AppModule { }
