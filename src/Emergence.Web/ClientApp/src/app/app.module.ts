import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { SpecimensListPageComponent } from './pages/specimens-list-page/specimens-list-page.component';
import { PlantInfosListPageComponent } from './pages/plant-infos-list-page/plant-infos-list-page.component';
import { SpecimenService } from './service/specimen-service';
import { SpecimenPageComponent } from './pages/specimen-page/specimen-page.component';
import { AboutComponent } from './pages/about/about.component';
import { TermsComponent } from './pages/terms/terms.component';
import { PrivacyComponent } from './pages/privacy/privacy.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    SpecimensListPageComponent,
    PlantInfosListPageComponent,
    SpecimenPageComponent,
    AboutComponent,
    TermsComponent,
    PrivacyComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    RouterModule.forRoot([
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'terms', component: TermsComponent },
    { path: 'privacy', component: PrivacyComponent },
    { path: 'about', component: AboutComponent },
    { path: 'specimens/list', component: SpecimensListPageComponent, canActivate: [AuthorizeGuard] },
    { path: 'plantinfos/list', component: PlantInfosListPageComponent, canActivate: [AuthorizeGuard] }], 
    { relativeLinkResolution: 'legacy' 
    })
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    { provide: SpecimenService, useClass: SpecimenService }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
