

function log(msg) {
  console.log(msg);

}

const getForgeryToken = () => {
  try {
    const forgeryToken = document.getElementsByName('__RequestVerificationToken')[0].value;
    return forgeryToken
  }
  catch (ex) {
    throw new Error("problem getting forgery token."+ex);
  }
}

function sortProductsBy() {
  const uri = "sortproducts";
  const sort = $('#sortby').val();

  sessionStorage.setItem("sortby", sort);
  $.ajax(uri,
    {
      data: { "sortBy": sort }
    }
  ).done((res) => {
    document.getElementById("products").innerHTML = res;
  });

  //document.getElementById("filterForm").submit();
}

function movePage() {
  const sort = $('#sortby').val();
  const page = $('#page').val();
  const lastId = $('.id').last().text();
}

function getBasket() {

  const uri = "getbasket"
  fetch(uri, { })
    .then(res => res.text())
    .then(r => {
      $('#basket').html(r);
      $('#basket').show();
      
    }

  );
      
}

function closeBasket() {
  $('#basket').hide();
}

 function updateBasketCount() {
  const uri = "getbasketcount";
   //$('#basketToggle').text(' ' + basketCount)
 
   fetch(uri, {
     method: "get"

   }).then(r => r.text()).then(r =>
     $('#basketToggle').text(' '+r)
   );
   
 
}



function setBasketItem(productId,isFromBasket) {
  
  
  const qt = !isFromBasket ? document.getElementById(productId+"-qt").value
  :document.getElementById(productId).value ;
  const forgeryToken = getForgeryToken();  
  const baseUrl = document.URL;
  const uri="setbasketitem"
  //const url = baseUrl + "addproduct";
  console.debug("test:{id}" + productId);
  
  $.ajax(uri, {

    method: "post",
    headers: { RequestVerificationToken: forgeryToken },
    data: { productId: productId,qt:qt }

  }
  ).done(updateBasketCount);
  
  //post(url, id);

}

function removeBasketItem(productId) {
  const uri = "removebasketitem";

  
  fetch(uri,
    {
      method: "post",
      headers: { "Content-Type": "application/json" },

      body: JSON.stringify(productId)

    }
  ).then(getBasket). then(decreaseBasketCount);
  

}

function decreaseBasketCount() {
  let basketToggle = $('#basketToggle');
  let basketCount = basketToggle.text();
  const newCount = parseInt(basketCount) - 1;
  basketToggle.text(' ' + newCount);
  //$('#basketToggle').text(' ' + basketCount)
}

function refreshBasket(html) {
  $('#basket').html(html);
}


function test() {
  const forgeryToken = getForgeryToken();
  const id = 3;
  const baseUrl = document.URL;
  const uri = "addproduct"
  const url = baseUrl + "addproduct";
  console.debug("test:{id}" + id);


}

/*
function increaseBasket() {
  let basketToggle = $('#basketToggle');
  let basketCount = basketToggle.text();
  const newCount = parseInt(basketCount)+1;
  basketToggle.text(' '+newCount);
  //$('#basketToggle').text(' ' + basketCount)
}

function decreaseBasket() {
  let basketToggle = $('#basketToggle');
  let basketCount = basketToggle.text();
  const newCount = parseInt(basketCount) - 1;
  basketToggle.text(' ' + newCount);
  //$('#basketToggle').text(' ' + basketCount)
}

function SubtractProduct(productId) {

  const forgeryToken = getForgeryToken();
  const baseUrl = document.URL;
  const uri = "addproduct"
  //const url = baseUrl + "addproduct";
  console.debug("test:{id}" + productId);

  $.ajax(uri, {

    method: "post",
    headers: { RequestVerificationToken: forgeryToken },
    data: { productId: productId }

  }
  );

  //post(url, id);

}

*/



