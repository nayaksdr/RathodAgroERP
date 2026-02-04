import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root',
})
export class AiAgentService {
    private api = `${environment.apiUrl}/api/ai-agent/ask`; 

  constructor(private http: HttpClient) {}

  ask(question: string) {
    return this.http.post<any>(this.api, { question });
  }
}