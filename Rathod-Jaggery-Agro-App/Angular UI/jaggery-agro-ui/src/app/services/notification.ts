import { Injectable } from '@angular/core';
import Swal, { SweetAlertIcon } from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  // A "Toast" configuration for non-intrusive alerts
  private Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    didOpen: (toast) => {
      toast.addEventListener('mouseenter', Swal.stopTimer);
      toast.addEventListener('mouseleave', Swal.resumeTimer);
    }
  });

  // Success Toast
  showSuccess(message: string) {
    this.Toast.fire({
      icon: 'success',
      title: message
    });
  }

  // Error Popup (Better for errors as they need attention)
  showError(message: string) {
    Swal.fire({
      icon: 'error',
      title: 'Oops...',
      text: message,
      confirmButtonColor: '#dc3545'
    });
  }
close() {
    Swal.close();
  }
  // 3. LOADING STATE (This fixes your error)
  // Purpose: Blocks the screen so users don't click "Save" twice
  showLoading(message: string = 'Processing...') {
    Swal.fire({
      title: message,
      allowOutsideClick: false,
      didOpen: () => {
        Swal.showLoading(); // This triggers the SweetAlert spinner
      }
    });
  }
  // Confirmation Dialog (Returns a Promise)
  async confirm(title: string, text: string, confirmButtonText: string = 'Yes, proceed') {
    return Swal.fire({
      title: title,
      text: text,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#0d6efd',
      cancelButtonColor: '#6c757d',
      confirmButtonText: confirmButtonText
    });
  }
}