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
import { SpecimensListComponent } from './pages/specimens/specimens-list/specimens-list.component';
import { PlantInfosListComponent } from './pages/plant-infos/plant-infos-list/plant-infos-list.component';
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
import { PhotoModalComponent } from './shared/components/photo-modal/photo-modal.component';
import { PlantInfoPageComponent } from './pages/plant-infos/plant-info-page/plant-info-page.component';
import { PlantInfoViewerComponent } from './pages/plant-infos/plant-info-viewer/plant-info-viewer.component';
import { PlantInfoEditComponent } from './pages/plant-infos/plant-info-edit/plant-info-edit.component';
import { OriginsListComponent } from './pages/origins/origins-list/origins-list.component';
import { OriginViewerComponent } from './pages/origins/origin-viewer/origin-viewer.component';
import { OriginEditComponent } from './pages/origins/origin-edit/origin-edit.component';
import { OriginPageComponent } from './pages/origins/origin-page/origin-page.component';
import { ActivitiesListComponent } from './pages/activities/activities-list/activities-list.component';
import { ActivityEditComponent } from './pages/activities/activity-edit/activity-edit.component';
import { ActivityPageComponent } from './pages/activities/activity-page/activity-page.component';
import { ActivityViewerComponent } from './pages/activities/activity-viewer/activity-viewer.component';
import { SpecimenModalComponent } from './pages/specimens/specimen-modal/specimen-modal.component';
import { OriginModalComponent } from './pages/origins/origin-modal/origin-modal.component';
import { UserPageComponent } from './pages/users/user-page/user-page.component';
import { ContactsPageComponent } from './pages/contacts/contacts-page/contacts-page.component';
import { ContactsListComponent } from './pages/contacts/contacts-list/contacts-list.component';
import { RequestsListComponent } from './pages/contacts/requests-list/requests-list.component';
import { InboxListComponent } from './pages/messages/inbox-list/inbox-list.component';
import { SentListComponent } from './pages/messages/sent-list/sent-list.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    SpecimensListComponent,
    PlantInfosListComponent,
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
    PagerComponent,
    PhotoModalComponent,
    PlantInfoPageComponent,
    PlantInfoViewerComponent,
    PlantInfoEditComponent,
    OriginsListComponent,
    OriginViewerComponent,
    OriginEditComponent,
    OriginPageComponent,
    ActivitiesListComponent,
    ActivityEditComponent,
    ActivityPageComponent,
    ActivityViewerComponent,
    SpecimenModalComponent,
    OriginModalComponent,
    UserPageComponent,
    ContactsPageComponent,
    ContactsListComponent,
    RequestsListComponent,
    InboxListComponent,
    SentListComponent
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
      { path: 'specimens/list', component: SpecimensListComponent, canActivate: [AuthorizeGuard],
        children: [
          { path: 'specimens/:id', component: SpecimenViewerComponent, canActivate: [AuthorizeGuard] },
          { path: 'specimens/edit/:id', component: SpecimenEditComponent, canActivate: [AuthorizeGuard] }
        ]},
      { path: 'specimens/:id', component: SpecimenViewerComponent, canActivate: [AuthorizeGuard] },
      { path: 'specimens/edit/:id', component: SpecimenEditComponent, canActivate: [AuthorizeGuard] },
      { path: 'plantinfos/list', component: PlantInfosListComponent, canActivate: [AuthorizeGuard],
      children: [
        { path: 'plantinfos/:id', component: PlantInfoViewerComponent, canActivate: [AuthorizeGuard] },
        { path: 'plantinfos/edit/:id', component: PlantInfoEditComponent, canActivate: [AuthorizeGuard] }
      ]},
      { path: 'plantinfos/:id', component: PlantInfoViewerComponent, canActivate: [AuthorizeGuard] },
      { path: 'plantinfos/edit/:id', component: PlantInfoEditComponent, canActivate: [AuthorizeGuard] },
      { path: 'origins/list', component: OriginsListComponent, canActivate: [AuthorizeGuard],
      children: [
        { path: 'origins/:id', component: OriginViewerComponent, canActivate: [AuthorizeGuard] },
        { path: 'origins/edit/:id', component: OriginEditComponent, canActivate: [AuthorizeGuard] }
      ]},
      { path: 'origins/:id', component: OriginViewerComponent, canActivate: [AuthorizeGuard] },
      { path: 'origins/edit/:id', component: OriginEditComponent, canActivate: [AuthorizeGuard] },
      { path: 'activities/list', component: ActivitiesListComponent, canActivate: [AuthorizeGuard],
      children: [
        { path: 'activities/:id', component: ActivityViewerComponent, canActivate: [AuthorizeGuard] },
        { path: 'activities/edit/:id', component: ActivityEditComponent, canActivate: [AuthorizeGuard] }
      ]},
      { path: 'activities/:id', component: ActivityViewerComponent, canActivate: [AuthorizeGuard] },
      { path: 'activities/edit/:id', component: ActivityEditComponent, canActivate: [AuthorizeGuard] },
      { path: 'user/:userName', component: UserPageComponent, canActivate: [AuthorizeGuard] },
      { path: 'contacts/list', component: ContactsListComponent, canActivate: [AuthorizeGuard] },
    ],
    { relativeLinkResolution: 'legacy' }),
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
