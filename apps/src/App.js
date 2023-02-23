import logo from './logo.svg';
import './App.css';

import {Home} from './Home';
import {Levels} from './Levels';
import {LevelDetails} from './LevelDetails';
import {ResourceDetails} from './ResourceDetails';
import {Navigation} from './Navigation';

import {BrowserRouter, Route, Routes} from 'react-router-dom';


function App() {
  return (
    <BrowserRouter>
      <div className="conatiner">
        <h3 className="m-3 d-flex justify-content-center">
            Project Bose
        </h3>
      </div>
      <Navigation/>

     <Routes>
       <Route path='/' element={<Home/>}/>
       <Route path='/Levels' element={<Levels/>}/>
       <Route path='/LevelDetails' element={<LevelDetails/>}/>
       <Route path='/ResourceDetails' element={<ResourceDetails/>}/>
     </Routes>
    </BrowserRouter>
  );
}

export default App;
