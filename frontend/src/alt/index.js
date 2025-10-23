
import { createStore } from 'vuex'; 
import axiosInstance from '@/axios';

// commit - wird genutzt um die Mutationen auszulösen 
// payload - einfaches Objekt um Daten zu übertragen
// it is recommented to use the Option Api here 
const store = createStore({
    state: {
        isAuthenticated: false,
        user: null    
    },
    mutations: {
        // sets the state of authentification and saves the user 
        setAuth(state, payload){
            state.isAuthenticated = payload.isAuthenticated;
            state.user = payload.user; 
        },
        // resets the state of authentication (Logout)
        logout(state) {
            state.isAuthenticated = false; 
            state.user = null; 
        } 
    }, 
    actions: {
        async login({ commit}, credentials) {
            try{
                const response = await axiosInstance.post('/account/login', credentials); 
                const userData = response.data; 
                //const userData = {username: credentials.username, email: 'user@example.com'}
                commit('setAuth', { isAuthenticated: true, user: userData });
                return response.data;
            } catch (error) {
                console.error('Login error:', error);
                throw error; 
            }

        },
        logout({ commit }){
            commit('logout'); 
        },     
    },
    getters: {
        isAuthenticated: state => state.isAuthenticated,
        user: state => state.user
    }
}); 

export default store; 