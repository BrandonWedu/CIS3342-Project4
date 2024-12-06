namespace Project4.Models
{
    internal static class SearchHomes
    {
        internal static Homes Search(Homes homes, string street, string city, States? state, string zipCode, int? priceMin, int? priceMax, PropertyType? propertyType, int? houseSizeMin, int? houseSizeMax, int? bedroomMin, double? bathroomMin, List<AmenityType> amenities, SaleStatus? saleStatus)
        {
            for (int i = 0; i < homes.List.Count; i++)
            {
                if (street != null)
                {
                    if (homes.List[i].Address.Street != street)
                    {
                        homes.RemoveAtIndex(i);
                        i--;
                        continue;
                    }
                }
                if (city != null)
                {
                    if (homes.List[i].Address.City != city)
                    {
                        homes.RemoveAtIndex(i);
                        i--;
                        continue;
                    }
                }
                if (state != null)
                {
                    if (homes.List[i].Address.State != state)
                    {
                        homes.RemoveAtIndex(i);
                        i--;
                        continue;
                    }
                }
                if (zipCode != null)
                {
                    if (homes.List[i].Address.ZipCode != zipCode)
                    {
                        homes.RemoveAtIndex(i);
                        i--;
                        continue;
                    }
                }
                if (priceMin != null)
                {
                    if (homes.List[i].Cost < priceMin)
                    {
                        homes.RemoveAtIndex(i);
                        i--;
                        continue;
                    }
                }
                if (priceMax != null)
                {
                    if (homes.List[i].Cost > priceMax)
                    {
                        homes.RemoveAtIndex(i);
                        i--;
                        continue;
                    }
                }
                if (propertyType != null)
                {
                    if (homes.List[i].PropertyType != propertyType)
                    {
                        homes.RemoveAtIndex(i);
                        i--;
                        continue;
                    }
                }
                if (houseSizeMin != null)
                {
                    if (homes.List[i].HomeSize < houseSizeMin)
                    {
                        homes.RemoveAtIndex(i);
                        i--;
                        continue;
                    }
                }
                if (houseSizeMax != null)
                {
                    if (homes.List[i].HomeSize > houseSizeMax)
                    {
                        homes.RemoveAtIndex(i);
                        i--;
                        continue;
                    }
                }
                if (bedroomMin != null)
                {
                    if (homes.List[i].Rooms.GetBedrooms() < (int)bedroomMin)
                    {
                        homes.RemoveAtIndex(i);
                        i--;
                        continue;
                    }
                }
                if (bathroomMin != null)
                {
                    double bathrooms = (double)homes.List[i].Rooms.GetHalfBaths() / 2.0 + homes.List[i].Rooms.GetFullBaths();
                    if (bathrooms < (double)bathroomMin)
                    {
                        homes.RemoveAtIndex(i);
                        i--;
                        continue;
                    }
                }
				if (amenities.Count > 0)
				{
					if (amenities.Count > homes.List[i].Amenities.List.Count)
					{
						homes.RemoveAtIndex(i);
						i--;
						continue;
					}

					bool shouldRemove = false;

					foreach (Amenity homeAmenity in homes.List[i].Amenities.List)
					{
						bool hasAmenity = false;

						foreach (AmenityType amenity in amenities)
						{
							if (amenity == homeAmenity.Type)
							{
								hasAmenity = true;
								break;
							}
						}

						if (!hasAmenity)
						{
							shouldRemove = true;
							break;
						}
					}

					if (shouldRemove)
					{
						homes.RemoveAtIndex(i);
						i--; // Adjust the index after removal
					}
				}


				if (saleStatus != null)
                {
                    if (homes.List[i].SaleStatus != saleStatus)
                    {
                        homes.RemoveAtIndex(i);
                        i--;
                        continue;
                    }
                }
            }
            return homes;
        }
    }
}
