import { Injectable } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';

@Injectable({
  providedIn: 'root'
})
export class SiteLayoutService {

  isSmall = false;
  isLarge = false;
  isMedium = false;

  constructor(private layoutObserver: BreakpointObserver) {

    // Handset
    // HandsetPortrait
    // HandsetLandscape

    // TabletPortrait
    // Tablet
    // TabletLandscape

    // Web
    // WebPortrait
    // WebLandscape

    layoutObserver.observe([
      Breakpoints.Handset,
      Breakpoints.HandsetPortrait,
      Breakpoints.HandsetLandscape,

    ]).subscribe(result => {
      if (result.matches) {
        this.isSmall = true;
        this.isLarge = false;
        this.isMedium = false;
      }
    });

    layoutObserver.observe([
      Breakpoints.Tablet,
      Breakpoints.TabletPortrait,
      Breakpoints.TabletLandscape,

    ]).subscribe(result => {
      if (result.matches) {
        this.isSmall = false;
        this.isLarge = false;
        this.isMedium = true;
      }
    });

    layoutObserver.observe([
      Breakpoints.Web,
      Breakpoints.WebPortrait,
      Breakpoints.WebLandscape,

    ]).subscribe(result => {
      if (result.matches) {
        this.isSmall = false;
        this.isLarge = true;
        this.isMedium = false;
      }
    });

  }
}
