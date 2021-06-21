import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

/**
 * Utility for presenting notifications
 */
@Injectable({
  providedIn: 'root'
})
export class NotifyService {

  constructor(private snackBar: MatSnackBar) {
  }

  public showNotification(message: string, duration = 4000) {
    this.snackBar.open(message, '', { duration: duration });
  }

  public showErrorNotification(message: string, duration = 4000) {
    this.snackBar.open(message, '', { duration: duration, panelClass: 'errorSnackbar' });
  }

  public showUnauthorizedNotification() {
    this.showErrorNotification('You are not authorized to access the requested resource');
  }
}
