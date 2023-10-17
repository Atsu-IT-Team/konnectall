let thumbnails = document.getElementsByClassName('thumbnail');
let slider = document.getElementById('slider');
let slider1 = document.getElementById('slider1');
let slider2 = document.getElementById('slider2');
let buttonRight = document.getElementById('slide-right');
let buttonLeft = document.getElementById('slide-left');

let buttonRight1 = document.getElementById('arrow-prev');
let buttonLeft1 = document.getElementById('arrow-next');

let buttonRight2 = document.getElementById('arrow-prev1');
let buttonLeft2 = document.getElementById('arrow-next1');


//console.log(buttonRight1);
buttonLeft?.addEventListener('click', function(){
    slider.scrollLeft -= 225;
    
})

buttonRight?.addEventListener('click', function(){
    slider.scrollLeft += 225;
   //");
})

buttonLeft1?.addEventListener('click', function(){
    slider1.scrollLeft -= 125;     
 })
 
buttonRight1?.addEventListener('click', function(){
    slider1.scrollLeft += 125;
    //");
 })
buttonLeft2?.addEventListener('click', function(){
    slider2.scrollLeft -= 125;     
 })
 
buttonRight2?.addEventListener('click', function(){
    slider2.scrollLeft += 125;
    //");
 })
/*const maxScrollLeft = slider.scrollWidth - slider.clientWidth;
const maxScrollLeft1 = slider1.scrollWidth - slider1.clientWidth;*/

const beverages = document.querySelectorAll('.beverages-brand');
const cosmetics = document.querySelectorAll('.cosmetics-brand');
const storeProducts = document.querySelectorAll('.store-product');
const storeCosmetic = document.querySelectorAll('.store-cosmetic');

for (i = 0; i < beverages.length; i++) {

    beverages[i].addEventListener('click', (e) => {
        e.preventDefault()
        var ele = e.target;

        if(ele.className=="img-brand"){
            ele = (ele.parentNode).id
        }
        else {
            ele = ((ele.parentNode).parentNode).id
        }
        storeProducts.forEach((product)=> {
            if (product.classList.contains(ele)){
                product.style.display = 'block'
            } else {
                product.style.display = 'none'
            }
        });
    });
};


for (i = 0; i < cosmetics.length; i++) {
    cosmetics[i].addEventListener('click', (e) => {
       
        e.preventDefault()
        var ele = e.target;
        if(ele.className=="img-brand"){
            ele = (ele.parentNode).id
        }
        else {
            ele = ((ele.parentNode).parentNode).id
        }
       
        storeCosmetic.forEach((product)=> {
            if (product.classList.contains(ele)){
                product.style.display = 'block'
            } else {
                product.style.display = 'none'
            }
        });
    });
};



