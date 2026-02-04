import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ProduceService {

  private api = `${environment.apiUrl}/api/produce`; 
  
  constructor(private http: HttpClient) {}

  getAll(from?: string, to?: string) {
  return this.http.get<any>(this.api, {
    params: {
      ...(from && { from }),
      ...(to && { to })
    }
  });
}


  create(data: any) {
    return this.http.post(this.api, data);
  }

  update(id: number, data: any) {
    return this.http.put(`${this.api}/${id}`, data);
  }

  delete(id: number) {
    return this.http.delete(`${this.api}/${id}`);
  }

  exportExcel(from?: string, to?: string) {
  let params = new HttpParams();

  if (from) {
    params = params.set('from', from);
  }

  if (to) {
    params = params.set('to', to);
  }

  return this.http.get(`${this.api}/export/excel`, {
    params,
    responseType: 'blob'
  });
}
}
