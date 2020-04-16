import { Component, OnInit, Output, ViewChild, ElementRef, Input } from '@angular/core';
import { FileService } from '../../../../services/files/file.service';
import { FileModel } from '../../../../models/data/data';
import { RespondData } from '../../../../models/base/utilities';
import { HttpErrorResponse } from '@angular/common/http';
import { EventEmitter } from '@angular/core';
import asEnumerable from 'linq-es2015';
import { AppConfig } from '../../../../services/config/app.config';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-file-uploader',
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.css']
})
export class FileUploaderComponent implements OnInit {
  @Input() files: FileModel[] = [];
  @Input() isReadonly = true;
  @Output() afterFileChange = new EventEmitter();
  @ViewChild('uploadFiles')
  uploadFilesHolder: ElementRef;
  fileDisplayUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.fileUrls.get;
  authTok:string;
  
  constructor(private _fileService: FileService, private _authService: AuthService) { }

  ngOnInit() {
    console.log("files: ", this.files);
    console.log("isReadonly: ", this.isReadonly);
    this.authTok = this._authService.getToken();
  }

  // submit file
  onFileChanged(event) {
    if (this.files == null) this.files = [];
    // const files = event.target.files;
    this._fileService.uploadFileMulti(event.target.files).subscribe((Response: RespondData) => {
      if (Response.isSuccess) {
        const recentlyCreatedFiles = Response.data;
        for (let i = 0; i < recentlyCreatedFiles.length; i++) {
          const el = recentlyCreatedFiles[i];
          el.isRecentlyCreated = true;
        }
        console.log('file-uploader type: ', Array.isArray(this.files));
        this.files = this.files.concat(recentlyCreatedFiles);
        this.afterFileChange.emit(this.files);
        console.log('file-uploader: ', this.files);
        console.log('file-uploader type: ', Array.isArray(this.files));
      }
    },
      (err: HttpErrorResponse) => { console.log(err); });
  }
  
  deleteFile(index: number) {
    // this.uploadFilesHolder.nativeElement.value = '';
    // Array.from(this.uploadFilesHolder.nativeElement.files).splice(index, 1);
    if (this.files[index].isRecentlyCreated) {
      this.files.splice(index, 1);
    } else {    
      this.files[index].isRecentlyDeleted = true;
    }
  }

  getDisplayFiles(files: FileModel[]) {
    return files.filter(x => !x.isRecentlyDeleted);
  }
}
