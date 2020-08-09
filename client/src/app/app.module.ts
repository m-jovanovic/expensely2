import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { ServiceWorkerModule } from '@angular/service-worker';
import { NgxsModule } from '@ngxs/store';
import { NgxsStoragePluginModule } from '@ngxs/storage-plugin';
import { environment } from '@env/environment';
import { CoreModule, AuthenticationState } from '@expensely/core';
import { MaterialModule } from '@expensely/material';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
    BrowserAnimationsModule,
    MaterialModule,
    CoreModule,
    NgxsModule.forRoot([AuthenticationState]),
    NgxsStoragePluginModule.forRoot({
      key: [AuthenticationState]
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
