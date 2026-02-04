import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class LanguageService {

  private api = `${environment.apiUrl}/api/language`; 

  constructor(private http: HttpClient) {}

  setLanguage(culture: string) {
    return this.http.post(`${this.api}/set`, {
      culture: culture,
      uiCulture: culture
    });
  }
}
