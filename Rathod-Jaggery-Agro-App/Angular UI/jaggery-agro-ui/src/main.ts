import { bootstrapApplication } from '@angular/platform-browser';
import { importProvidersFrom } from '@angular/core';
import { provideHttpClient, withInterceptors, HttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';

import { AppComponent } from './app/app';
import { routes } from './app/app.routes';
import { jwtInterceptor } from './app/interceptors/jwt-interceptor';

import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { Observable } from 'rxjs';

/* ✅ CUSTOM TRANSLATE LOADER */
export function HttpLoaderFactory(http: HttpClient): TranslateLoader {
  return {
    getTranslation: (lang: string): Observable<any> =>
      http.get(`./assets/i18n/${lang}.json`)
  };
}

bootstrapApplication(AppComponent, {
  providers: [
    // ✅ ROUTER (REQUIRED)
    provideRouter(routes),

    // ✅ HTTP + JWT
    provideHttpClient(
      withInterceptors([jwtInterceptor])
    ),

    // ✅ TRANSLATION
    importProvidersFrom(
      TranslateModule.forRoot({
        loader: {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient]
        }
      })
    )
  ]
});
