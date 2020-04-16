import { Injectable } from "@angular/core";
import { BaseDataService } from "../base/base-data-service";

 @Injectable({
    providedIn: 'root'
})
export class LocalStorageService {    
    constructor() {}

    store(key, value) {
        localStorage.setItem(key, JSON.stringify(value));
    }

    get(key): any {
        return JSON.parse(localStorage.getItem(key));
    }
}