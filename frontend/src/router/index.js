// Define our Routing rules 
import {createRouter, createWebHistory} from "vue-router";
import { useAuthStore } from "@/store/authStore"
import axiosInstance from "@/axios";

import HomeView from "../views/HomeView.vue";
import LoginView from "@/views/LoginView.vue";
import AccountView from "@/views/AccountView.vue"; 
import GrünesKraftwerkView from "@/views/GrünesKraftwerkView.vue";
import MaterialDatenbankView from "@/views/MaterialDatenbankView.vue";
import SDGView from "@/views/SDGView.vue";
import CompanyView from "@/views/CompanyView.vue";
// import CreateSdgView from "@/views/CreateSdgView.vue"; 
import AdminDashboardView from '../views/AdminDashboardView.vue'; 

import CompaniesOverviewView from "@/views/CompaniesOverviewView.vue";
import MaterialMarktplatzView from "@/views/MaterialMarktplatzView.vue";


const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: "/", 
            name: "home",
            component: HomeView
        },
        {
            path: "/login",
            name: "login", 
            component: LoginView
        },
        {
            path: "/account",
            name: "account",
            component: AccountView
        },
        {
            path: "/kraftwerk",
            name: "GrünesKraftwerk",
            component: GrünesKraftwerkView
        },
        {
            path: "/materialdatenbank",
            name: "MaterialDatenbank",
            component: MaterialDatenbankView,
            meta:{ requiresAuth: true}
        },
        {
            path: "/sdgs",
            name: "SDGs",
            component: SDGView
        },
        {
            path: "/company/:id",
            name: "CompanyView",
            component: CompanyView,
            props: true,
        },
        {
            path: "/companies",
            name: "CompaniesOverview",
            component: CompaniesOverviewView
        },
        {
            path: "/admin-dashboard",
            name: "AdminDashboard",
            component: AdminDashboardView,
            meta:{ requiresAuth: true} // Diese Route benötigt eine Authentifizierung
        },
        {
            path: "/materialmarktplatz",
            name: "MaterialMarktplatz",
            component: MaterialMarktplatzView,
            meta:{ requiresAuth: true} // Diese Route benötigt eine Authentifizierung
        },
    ]
})

// Navigation Guard
// to - Feld der aktuellen Route
// from - 
// next - 
// meta - 
// 
router.beforeEach(async (to, from, next) => {
    const authStore = useAuthStore(); 
    // Überprüfen, ob Authentifizierung erforderlich ist
    const requiresAuth = to.matched.some(record => record.meta.requiresAuth); 
    
    if (requiresAuth && !authStore.isAuthenticated) {
        try {
            const response = await axiosInstance.get('/account/me'); 
            
            authStore.setAuth(true, response.data); 
            next(); 
        } catch (error) {
            authStore.logout();
            next({
                path: '/login', 
                query: { redirect: to.fullPath } // Nach dem Login zurück zur ursprünglichen Seite
            });
        }
    } else {
        next(); 
    }
});

export default router; 