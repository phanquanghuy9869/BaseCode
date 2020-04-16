import { Injectable } from '@angular/core';
import { BaseDataService } from '../base/base-data-service';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../config/app.config';

@Injectable({
  providedIn: 'root'
})
export class FileService extends BaseDataService{
  private uploadFileUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.fileUrls.upload;
  private getFileUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.fileUrls.get;
  
  constructor(protected httpClient: HttpClient) {
    super(httpClient);
}

  uploadFileMulti(files: File[]) {
    const uploadData = new FormData();
    for (let i = 0; i < files.length; i++) {
      const file = files[i];
      uploadData.append('files', file, file.name);
    }
    return this.httpClient.post(this.uploadFileUrl, uploadData);
  }
}
