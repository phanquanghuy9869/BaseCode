import { Injectable } from '@angular/core';
import { SwUpdate } from '@angular/service-worker';
import { interval } from 'rxjs/internal/observable/interval';

@Injectable({
  providedIn: 'root'
})
export class SwUpdateService {

  constructor(public updates: SwUpdate) {   
    console.log(this.updates.isEnabled);
    if (this.updates.isEnabled) {
      // this.updates.activateUpdate();
      interval(20000).subscribe(() => this.updates.checkForUpdate()
        .then(() => console.log('checking for updates')));
    }
  }

  public checkForUpdates(): void {
    this.updates.available.subscribe(event => this.promptUser());
    this.updates.checkForUpdate();
  }

  private promptUser(): void {
    console.log('updating to new version');
    this.updates.activateUpdate().then(() => document.location.reload());
  }
}
