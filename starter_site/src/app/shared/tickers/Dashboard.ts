import { Tab } from './Tab';
import { TickerService } from './ticker.service';

export class Dashboard {

    public tabs: Tab[];
    public activeTabIndex = 0;

    constructor(public tickerService: TickerService) {
        this.tabs = [];
        // this.addTab();
    }

    initialize(d: any) {
        for (let x = 0; x < d.tabs.length; x++) {
            const t = new Tab(d.tabs[x].title, [], d.tabs[x].active, this.tickerService);
            t.buildTabFromData(d.tabs[x]);
            this.tabs.push(t);

            if (d.tabs[x].active) {
                this.setActiveTab(t);
            }
        }
    }

    activeTab() {
        return this.tabs[this.activeTabIndex];
    }

    setActiveTab(tab: Tab) {

        if (tab == null) {
            return;
        }

        this.tabs.forEach(t => { t.deactivate(); });

        if (tab != null) {
            this.activeTabIndex = this.tabs.indexOf(tab);
            tab.activate();
        } else {
            this.tabs[0].activate();
            this.activeTabIndex = 0;
        }
    }

    addTab(): string {

        if (this.tabs.length >= 10) {
            return 'Can\'t add another tab';
        }

        this.tabs.forEach(t => { t.deactivate(); });
        this.tabs.push(new Tab('New Tab', [], true, this.tickerService));
        this.activeTabIndex = this.tabs.length - 1;

        return '';
    }

    deleteTab(tab: Tab): string {

        const index = this.tabs.indexOf(tab);

        if (index === -1) {
            return 'Tab not found!';
        }

        if (this.tabs.length === 1) {
            return 'Can\'t delete the last tab';
        }

        this.tabs.splice(index, 1);
        this.tabs.forEach(t => { t.deactivate(); });


        this.tabs[index === 0 ? 0 : index - 1].activate();
        this.activeTabIndex = index === 0 ? 0 : index - 1;

        return '';

    }



    getSerializedContent(): string {
        // go through every tab and every ticker in that tab and get a serialized representation of it.
        const res = { activeTabIndex: -1, tabs: []};
        for (let x = 0; x < this.tabs.length; x++) {
            res.tabs.push(this.tabs[x].getDataObject());
        }

        return JSON.stringify(res);

    }
}
