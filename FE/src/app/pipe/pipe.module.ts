
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

//import { TranslateService } from './translate.service';
import { TranslatePipe } from './translate.pipe';
import { MoneyFormatPipe } from './money-format.pipe';


@NgModule({
    imports: [HttpClientModule],
    declarations: [TranslatePipe, MoneyFormatPipe],
    exports: [TranslatePipe, MoneyFormatPipe],
    providers: [],
})
export class PipeModule { }


