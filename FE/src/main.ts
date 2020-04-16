import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import { version } from 'punycode';

if (environment.production) {
  enableProdMode();
  console.log('1.01');
}

// platformBrowserDynamic().bootstrapModule(AppModule)
//   .catch(err => console.error(err));
platformBrowserDynamic().bootstrapModule(AppModule).then(() => {
  if ('serviceWorker' in navigator && environment.production) {
    navigator.serviceWorker.register('ngsw-worker.js');
  }
}).catch(err => console.log(err));

