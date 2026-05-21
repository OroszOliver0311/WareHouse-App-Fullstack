import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { API_BASE_URL, InventoryClient, ProductsClient, StockMovementsClient, WareHousesClient } from './api/api-client';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(),
    { provide: API_BASE_URL, useValue: 'https://localhost:7122' }, 
    InventoryClient,
    ProductsClient,
    StockMovementsClient,
    WareHousesClient
  ]
};