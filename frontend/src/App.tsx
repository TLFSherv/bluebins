import { Routes, Route } from 'react-router-dom'
import Home from './Recyclers/Pages/Home'
import SignUp from './Recyclers/Pages/Signup'
import SignIn from './Recyclers/Pages/Signin'
import Layout from './Recyclers/Layout'

function App() {
  return (
    <Layout>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/sign-up" element={<SignUp />} />
        <Route path="/sign-in" element={<SignIn />} />
      </Routes>
    </Layout>
  )
}

export default App