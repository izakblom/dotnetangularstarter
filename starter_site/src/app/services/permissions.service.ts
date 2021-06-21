import { Injectable } from '@angular/core';
import { Permission } from '../shared/models/permission';
import { ApiService } from './api.service';
import { FeatureGuard } from '../shared/models/featureGuard';

@Injectable({
  providedIn: 'root'
})
export class PermissionsService {

  permissions: Permission[];
  featureGuards: FeatureGuard[];

  constructor(private apiService: ApiService) { }

  getAllPermissions(): Promise<Permission[]> {
    return new Promise((resolve, reject) => {
      if (this.permissions) {
        resolve(this.permissions);
      } else {
        this.apiService.get('api/home/GetAllPermissions').subscribe(result => {
          this.permissions = result;
          resolve(this.permissions);
        }, error => {
          reject(error);
        });
      }
    });

  }

  getAllFeatureGuards(): Promise<FeatureGuard[]> {
    return new Promise((resolve, reject) => {
      if (this.featureGuards) {
        resolve(this.featureGuards);
      } else {
        this.apiService.get('api/home/GetAllFeatureGuards').subscribe(result => {
          this.featureGuards = result;
          resolve(this.featureGuards);
        }, error => {
          reject(error);
        });
      }
    });
  }


  async findFeatureMappingByFeatureName(featName: string): Promise<FeatureGuard> {
    try {
      let result: FeatureGuard;
      if (!this.featureGuards) {
        await this.getAllFeatureGuards();
      }
      result = this.featureGuards.find(feat => feat.name === featName);
      return result;
    } catch (error) {
      return null;
    }

  }
}
