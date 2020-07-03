# Manage Inventory

## Stories
- As a propagator, I want to add a plant to my inventory so I can track its progress.
  - [Plant as Inventory](../../../../../emergence/projects/2#card-41209456) As a propagator, I want to be able to add plant profile data to the item in my inventory so I don't have to look up how to to grow it. 
  - [Plant Profile Source](../../../../../emergence/projects/2#card-41209690) As a propagator, I want to be able to be able to track where I got the plant profile data from so I can credit the source 
- As a propagator, I want to know what seeds I have available so I can sow them.
  - As a propagator, I want to see all of my inventory in a list so I keep everything up to date.
  - As a propagator, I want to search my inventory by keyword so I can find a specific specimen.
  - As a propagator, I want to filter my inventory by keyword so I can limit the results to find what I'm looking for and I don't know the specifics.
- As a propagator, I want to know what plants I have that are ready to put into the ground.
- As a propagator, I want to know if I have any space for my seeds that are ready to germinate.
- As a propagator, I want to know if the seeds I have are expiring so I can use them before they expire.
- As a propagator, I want to know where a seed packet came from so that if I got a bad rate of germination, I can avoid that supplier, or traversely a good rate so I can use that supplier.
- As a propagator, I want to let other propogators know if I have any inventory available for trade.
- As a propagator, I want to check my wishlist when I'm shopping so I can acquire items I want.

## Data design
- Item
  - Name
  - Type
    - Specimen - an instance of a plant
      - Plant - profile of a plant
    - Supply
    - Capacity
      - Pots, trays, space?
 - Status
 - DateAcquired
 - Source
   - Source
   - A source can has a parent and children
   - Type:
     - Nursery
     - Store
     - Location
     - Person
     - Event
   - Contact
   - URL
 - IsWishlist
- Examples:
  - Root hormone
  - Packet of Joe Pye Weed seeds
  - Collected seeds
  - Bag of mushroom compost
  - Max-sea

- Is a seed considered a specimen?
  - Yes
