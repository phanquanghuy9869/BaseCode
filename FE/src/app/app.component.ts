import { Component } from '@angular/core';
import { SwUpdateService } from 'src/app/services/worker/sw-update/sw-update.service';
import { VscService } from 'src/app/services/vsc/vsc.service';
import { AppConfig } from 'src/app/services/config/app.config';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'kpi-frontend';

  private vscUrl = AppConfig.settings.vscUrls.versionCheck;
  constructor(private _vscService: VscService) {
    this._vscService.initVersionCheck(this.vscUrl, 1000 * 60 * 3);
  }
}
