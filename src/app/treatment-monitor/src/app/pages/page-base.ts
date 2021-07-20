import {Location} from '@angular/common';
import {DialogService} from '../services/dialog.service';

export abstract class PageBase {
  protected constructor(protected dialogService: DialogService,
                        protected location: Location) {
  }

  protected showErrorDialog(error: any, dialogTitle: string): void {
    if (!error) {
      return;
    }

    let message = 'Unexpected error occurred.';
    if (error.error && error.error.errors && error.error.errors['']) {
      message = '';
      // @ts-ignore
      error.error.errors[''].forEach(item => message += item + ' ');
    } else if (error.error && error.error.errors && error.error.errors.length > 0) {
      message = '';
      // @ts-ignore
      for (const k in error.error.errors){
        if (error.error.errors.hasOwnProperty(k)){
          message += error.error.errors[k] + ' ';
        }
      }
    }

    this.dialogService.showMessage(dialogTitle, message, 'error');
  }

  public goBack(): void {
    this.location.back();
  }

  protected buildFormData(formData: FormData, data: any, parentKey: string): void {
    if (data && typeof data === 'object' && !(data instanceof Date) && !(data instanceof File)) {
      Object.keys(data).forEach(key => {
        this.buildFormData(formData, data[key], parentKey ? `${parentKey}[${key}]` : key);
      });
    } else {
      const value = data == null ? '' : data;

      formData.append(parentKey, value);
    }
  }
}
