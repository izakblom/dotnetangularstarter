import { Component, OnInit, Input, ViewChildren, QueryList, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { CdkDropList, CdkDragDrop, CdkDragEnter, moveItemInArray } from '@angular/cdk/drag-drop';
import { SiteLayoutService } from 'src/app/services/layout-service.service';
import { Ticker } from '../Ticker';
import { Dashboard } from '../Dashboard';
import { NotifyService } from 'src/app/services/notifyService';
import { Tab } from '../Tab';
import { TickerService } from '../ticker.service';
import { TickerTemplate } from '../TickerTemplate';

import { DashboardContent } from '../DashboardContent';

@Component({
  selector: 'app-ticker-dashboard',
  templateUrl: './ticker-dashboard.component.html',
  styleUrls: ['./ticker-dashboard.component.css']
})
export class TickerDashboardComponent implements OnInit, AfterViewInit {

  @Input() route: string;

  @ViewChildren(CdkDropList) dropsQuery: QueryList<CdkDropList>;

  drops: CdkDropList[];
  dashboard: Dashboard;

  editingTabTitle = false;
  editingTitle = '';

  public availableTickers: TickerTemplate[] = [];

  constructor(
    public layout: SiteLayoutService,
    private notifyService: NotifyService,
    private tickerService: TickerService,
    private cd: ChangeDetectorRef) {

    this.dashboard = new Dashboard(this.tickerService);
  }

  ngOnInit() {

    this.tickerService.getUserDashboard(this.route)
      .subscribe(
        (resp: any) => {
          if (!resp.exists) {
            this.dashboard.addTab();
          } else {
            const d = JSON.parse(resp.serializedDashboard);
            this.dashboard.initialize(d);
          }

        },
        (err: any) => {
          // console.log('error dashboard');
          // console.log(err);
        }
      );

    this.tickerService.getTickers().subscribe(
      (resp: any) => {
        this.availableTickers = resp;
        this.cd.detectChanges();
      },
      (err: any) => {
        this.notifyService.showErrorNotification(err);
      }
    );
  }

  ngAfterViewInit() {
    this.activate(this.dashboard.tabs[0]);
  }

  entered($event: CdkDragEnter) {
    // console.log($event.item.data, $event.container.data);
    moveItemInArray(this.dashboard.activeTab().tickers, $event.item.data, $event.container.data);
  }

  addTicker(tab: Tab) {

    if (!this.availableTickers || this.availableTickers.length === 0) {
      this.notifyService.showErrorNotification('No ticker template available.');
      return;
    }

    const res = tab.addTicker(this.availableTickers[Object.keys(this.availableTickers)[0]][0]);

    if (res !== '') {
      this.notifyService.showErrorNotification(res);
    } else {
      this.notifyService.showNotification('Ticker added!');
    }
  }

  addTab() {
    const res = this.dashboard.addTab();

    if (res !== '') {
      this.notifyService.showErrorNotification(res);
    } else {
      this.notifyService.showNotification('Tab added!');
    }
  }

  deleteTab(tab: Tab) {
    const res = this.dashboard.deleteTab(tab);

    if (res !== '') {
      this.notifyService.showErrorNotification(res);
    } else {
      this.notifyService.showNotification('Tab deleted!');
    }
  }

  deleteTicker(ticker: Ticker) {

    const res = this.dashboard.activeTab().deleteTicker(ticker);

    if (res !== '') {
      this.notifyService.showErrorNotification(res);
    } else {
      this.notifyService.showNotification('Ticker deleted!');
    }

  }

  refreshTicker(ticker: Ticker) {

    this.dashboard.activeTab().toggleTickerLoading(ticker, true);

    this.tickerService.getTickerData(ticker.filter)
      .subscribe(
        (resp: any) => {
          this.dashboard.activeTab().refreshTicker(ticker, resp.count);
          this.dashboard.activeTab().toggleTickerLoading(ticker, false);
        },
        (err: any) => {
          // console.log(err);
          this.dashboard.activeTab().toggleTickerLoading(ticker, false);
        }
      );
  }

  configureTicker(ticker: Ticker) {
    this.dashboard.activeTab().configureTicker(ticker);
  }

  cancelTickerConfiguration() {
    this.dashboard.activeTab().cancelTickerConfiguration();
  }

  saveTickerConfiguration(ticker: Ticker) {
    // console.log('Ticker configuration being saved');
    // console.log(ticker);
    const res = this.dashboard.activeTab().saveTickerConfiguration(ticker);

    if (res !== '') {
      this.notifyService.showErrorNotification(res);
    } else {
      this.notifyService.showNotification('Ticker updated!');
    }
  }

  openRenameTab() {
    this.editingTabTitle = true;
    this.editingTitle = this.dashboard.activeTab().title;
  }

  updateTabName() {
    this.editingTabTitle = false;
    this.dashboard.activeTab().title = this.editingTitle;
  }

  cancelTabName() {
    this.editingTabTitle = false;
    this.editingTitle = '';
  }

  activate(tab: Tab) {
    this.dashboard.setActiveTab(tab);

    this.dropsQuery.changes.subscribe(() => {
      this.drops = this.dropsQuery.toArray();
    });

    Promise.resolve().then(() => {
      this.drops = this.dropsQuery.toArray();
      // console.log(this.drops);
    });

  }

  saveDashboard() {

    const d = new DashboardContent();

    d.route = this.route;
    d.dashboard = this.dashboard.getSerializedContent();

    this.tickerService.updateUserDashboard(d)
      .subscribe(
        (resp: any) => {
          this.notifyService.showNotification('Your dashboard is saved!');
        },
        (err: any) => {
          this.notifyService.showErrorNotification(err);
        }
      );
  }

}
