import {useAuth} from 'react-oidc-context';
import {useEffect, useRef} from 'react';
import axios from 'axios';

export const useCreateUser = () => {
    const auth = useAuth();
    const syncedRef = useRef(false);

   
    useEffect(() => {
        if (auth.isAuthenticated && auth.user && !syncedRef.current) {
            const userData = {
                sub: auth.user.profile.sub,
                email: auth.user.profile.email,
                name: auth.user.profile.name,
                given_name: auth.user.profile.given_name,
                family_name: auth.user.profile.family_name,
                preferred_username: auth.user.profile.preferred_username,
            };
            const syncUser = async () => {
                try {
                    await axios.post('/api/users/create', userData, {
                        headers: {
                            Authorization: `Bearer ${auth.user.access_token}`,
                        },
                    });

                    console.log('✅ User synced to backend');
                    syncedRef.current = true;

                } catch (error) {
                    console.error('❌ User sync failed:', error);
                }
            };

            syncUser();
        }
    }, [auth.isAuthenticated, auth.user]);

};


