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
import { SpecimensListPageComponent } from './pages/specimens/specimens-list-page/specimens-list-page.component';
import { PlantInfosListPageComponent } from './pages/plant-infos-list-page/plant-infos-list-page.component';
import { SpecimenService } from './service/specimen-service';
import { SpecimenPageComponent } from './pages/specimens/specimen-page/specimen-page.component';
import { AboutComponent } from './pages/about/about.component';
import { TermsComponent } from './pages/terms/terms.component';
import { PrivacyComponent } from './pages/privacy/privacy.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SortableHeadersComponent } from './shared/components/sortable-headers/sortable-headers.component';
import { SpecimenViewerComponent } from './pages/specimens/specimen-viewer/specimen-viewer.component';
import { SearchFiltersComponent } from './shared/components/search-filters/search-filters.component';
import { ContentCardComponent } from './shared/components/content-card/content-card.component';
import { SpecimenEditComponent } from './pages/specimens/specimen-edit/specimen-edit.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { OriginService } from './service/origin-service';
import { LifeformService } from './service/lifeform-service';
import { StorageService } from './service/storage-service';
import { UploadPhotosComponent } from './shared/components/upload-photos/upload-photos.component';
import { ViewPhotosComponent } from './shared/components/view-photos/view-photos.component';
import { PhotoService } from './service/photo-service';
import { PagerComponent } from './shared/components/pager/pager.component';

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
    PrivacyComponent,
    SortableHeadersComponent,
    SpecimenViewerComponent,
    SearchFiltersComponent,
    ContentCardComponent,
    SpecimenEditComponent,
    UploadPhotosComponent,
    ViewPhotosComponent,
    PagerComponent
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
    { path: 'specimens/list', component: SpecimensListPageComponent, canActivate: [AuthorizeGuard],
      children: [
        { path: 'specimens/:id', component: SpecimenViewerComponent, canActivate: [AuthorizeGuard] }, 
        { path: 'specimens/edit/:id', component: SpecimenEditComponent, canActivate: [AuthorizeGuard] }
      ]},
    { path: 'specimens/:id', component: SpecimenViewerComponent, canActivate: [AuthorizeGuard] }, 
    { path: 'specimens/edit/:id', component: SpecimenEditComponent, canActivate: [AuthorizeGuard] },
    { path: 'plantinfos/list', component: PlantInfosListPageComponent, canActivate: [AuthorizeGuard] }], 
    { relativeLinkResolution: 'legacy' 
    }),
    BrowserAnimationsModule,
    NgbModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    { provide: SpecimenService, useClass: SpecimenService },
    { provide: LifeformService, useClass: LifeformService },
    { provide: OriginService, useClass: OriginService },
    { provide: PhotoService, useClass: PhotoService },
    { provide: StorageService, useClass: StorageService }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
