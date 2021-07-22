import { Injectable } from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {Observable} from "rxjs";
import {LoaderDialogComponent} from "../dialogs/loader-dialog/loader-dialog.component";
import {MessageDialogComponent} from "../dialogs/message-dialog/message-dialog.component";
import {ConfirmationDialogComponent} from "../dialogs/confirmation-dialog/confirmation-dialog.component";

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private dialog: MatDialog) { }

  callWithLoader<T>(action: () => Observable<T>): Observable<T> {
    const dRef = this.dialog.open(LoaderDialogComponent, {
      disableClose: true
    });

    return new Observable<T>(subscriber => {
      action().subscribe(value => {
        dRef.close();
        dRef.afterClosed().subscribe(() => {
          subscriber.next(value);
        });
      }, error => {
        dRef.close();
        dRef.afterClosed().subscribe(() => {
          subscriber.error(error);
        });
      });
    });
  }

  showMessage(title: string, message: string, type: string, messageClosed?: () => void): void {
    const dRef = this.dialog.open(MessageDialogComponent, {
      disableClose: true,
      data: {
        title,
        message,
        type
      }
    });
    dRef.afterClosed().subscribe(() => {
      if (messageClosed) {
        messageClosed();
      }
    });
  }

  showConfirmation(title: string, message: string, messageClosed?: (result: boolean) => void): void {
    const dRef = this.dialog.open(ConfirmationDialogComponent, {
      disableClose: true,
      data: {
        title,
        message
      }
    });
    dRef.afterClosed().subscribe((result) => {
      if (messageClosed) {
        messageClosed(result);
      }
    });
  }

  closeAll(): void {
    this.dialog.closeAll();
  }
}
