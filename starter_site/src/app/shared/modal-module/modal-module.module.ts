import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { YesNoModalComponent } from './yes-no-modal/yes-no-modal.component';
import { ClarityModule } from '@clr/angular';

@NgModule({
  declarations: [YesNoModalComponent],
  imports: [
    CommonModule,
    ClarityModule
  ],
  exports: [YesNoModalComponent]
})
export class ModalModule { }
