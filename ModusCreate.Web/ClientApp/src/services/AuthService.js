import jwt_decode from 'jwt-decode';

class Auth{
    constructor(){
        this.user = null;
    }

    async login(email, password){
        if (this.isAuthenticated())
            return;

        const url = 'api/auth/login';

        const response = await fetch(url, {
            method:'post',
            headers: {
              'Accept': 'application/json',
              'Content-Type': 'application/json'
            },
            body: JSON.stringify({email: email, password:password})
        });
        
        if (response.ok){
            const token = await response.json();
            const tokenInfo = jwt_decode(token.jwt);
            this.user = {...tokenInfo, token:token};

            localStorage.setItem('user', JSON.stringify(this.user));
        }else{
            throw (await response.text());
        }
      }

      logoff(){
        localStorage.removeItem('user');
        this.user = null;
      }
      
      isAuthenticated(){
          return (this.getUser()!= null);
      }

      isExpired(user){
        const d = new Date(0);
        d.setUTCSeconds(user.exp);

        return d <= new Date();
      }

      getUser(){
        if (this.user == null){
            const user = localStorage.getItem('user');
            if (user != null)
                this.user = JSON.parse(user);
        } 

        if (this.user == null || this.isExpired(this.user)){
            localStorage.removeItem('user');
            this.user = null;
        }

        return this.user;
      }
}

const AuthService = new Auth();

export default AuthService;