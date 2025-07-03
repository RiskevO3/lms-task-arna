window.loginUser = async function (loginData) {
  try {
    const response = await fetch("/api/auth/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: loginData,
    })

    return response.ok
  } catch (error) {
    console.error("Login error:", error)
    return false
  }
}
