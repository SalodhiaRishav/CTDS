import HttpClient from "./../Utils/HttpRequestWrapper";
import store from "./../store/store";
import router from "./../router/index";

export default function exceptionResponse() {
  HttpClient.http.interceptors.response.use(
    function(response) {
      if (isTokenExpired(response)) {
        return new Promise((resolve, reject) => {
          refreshToken()
            .then(res => {
              if (res.isTokenRefreshed) {
                resolve({ data: "token refreshed" });
              }
            })
            .catch(err => {
              reject(err);
            });
        });
      } else {
        return response;
      }
    },
    function(error) {
      errorHandler(error);
    }
  );
}

const errorHandler = error => {
  if (401 === error.response.status) {
    alert("Unauthorized access,redirecting to login");
    router.push("/login");
    return;
  } else if (500 === error.response.status) {
    alert("internal server error,redirecting to login");
    router.push("/login");
    return;
  }
};

const refreshToken = () => {
  return new Promise((resolve, reject) => {
    const refreshTokenSerial = sessionStorage.getItem("refreshTokenId");
    store
      .dispatch("getNewToken", refreshTokenSerial)
      .then(response => {
        if (response) {
          sessionStorage.setItem("accessToken", response);
          resolve({ isTokenRefreshed: true, token: response });
        }
      })
      .catch(error => {
        reject(error);
      });
  });
};

const isTokenExpired = response => {
  if (response.data === "token expired") {
    return true;
  }
  return false;
};
