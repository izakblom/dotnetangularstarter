import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

/**
 * Simple component to present a modal with yes/no buttons
 */
@Component({
  selector: 'app-yes-no-modal',
  templateUrl: './yes-no-modal.component.html',
  styleUrls: ['./yes-no-modal.component.css']
})
export class YesNoModalComponent implements OnInit {
  @Input() title: string;
  @Input() body: string;
  @Input() yesBtnText = 'Yes';
  @Input() noBtnText = 'No';
  @Output() optionSelected = new EventEmitter<boolean>();
  public show = false;
  private yesCallback: () => void;
  private noCallback: () => void;

  constructor() { }

  ngOnInit() {
  }

  onNo() {
    this.optionSelected.emit(false);
    this.show = false;
    if (this.noCallback) {
      this.noCallback();
    }
  }

  onYes() {
    this.optionSelected.emit(true);
    this.show = false;
    if (this.yesCallback) {
      this.yesCallback();
    }
  }

  /**
   * Show the modal. Optionally provide callback functions to handle yes/no actions
   * @param yesCallback Callback function to handle yes action
   * @param noCallback Callback function to handle no action
   * @param title Optional title override
   * @param body Optional body override
   * @param yesBtnText Optional yesBtnText
   * @param noBtnText Optional noBtnText
   */
  showModal(yesCallback?: () => void, noCallback?: () => void, title?: string, body?: string, yesBtnText?: string, noBtnText?: string) {
    if (title) {
      this.title = title;
    }
    if (body) {
      this.body = body;
    }
    if (yesBtnText) {
      this.yesBtnText = yesBtnText;
    }
    if (noBtnText) {
      this.noBtnText = noBtnText;
    }
    this.show = true;
    this.yesCallback = yesCallback;
    this.noCallback = noCallback;
  }
}
