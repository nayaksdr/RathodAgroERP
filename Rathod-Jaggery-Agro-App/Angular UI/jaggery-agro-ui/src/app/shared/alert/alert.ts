import Swal from 'sweetalert2';

export class Alert {

  static confirm(title = 'Are you sure?', text = 'You won’t be able to revert this!') {
    return Swal.fire({
      title,
      text,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#198754',
      cancelButtonColor: '#dc3545',
      confirmButtonText: 'Yes',
      cancelButtonText: 'Cancel'
    });
  }

  static success(message: string) {
    Swal.fire({
      icon: 'success',
      title: 'Success',
      text: message,
      timer: 1500,
      showConfirmButton: false
    });
  }

  static error(message: string) {
    Swal.fire({
      icon: 'error',
      title: 'Error',
      text: message
    });
  }
}
